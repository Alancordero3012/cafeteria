using System.Collections.ObjectModel;
using CafeteriaSystem.Application.Services;
using CafeteriaSystem.Domain.Entities;
using CafeteriaSystem.Domain.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CafeteriaSystem.WPF.ViewModels;

public partial class CategoriesViewModel(
    ICategoryRepository categoryRepository,
    ActivityLogService logService,
    AuthService authService) : BaseViewModel
{
    [ObservableProperty] private ObservableCollection<Category> _categories = [];
    [ObservableProperty] private Category? _selectedCategory;
    [ObservableProperty] private bool _isEditing;

    // Campos del formulario
    [ObservableProperty] private string _editName = string.Empty;
    [ObservableProperty] private string _editDescription = string.Empty;
    [ObservableProperty] private string _editColor = "#E8A87C";
    [ObservableProperty] private bool _editIsActive = true;
    [ObservableProperty] private string _formTitle = "Nueva Categoría";

    // Paleta de colores predefinida para la UI
    public static IReadOnlyList<string> ColorPalette =>
    [
        "#E8A87C", "#4FC3F7", "#4CAF50", "#F48FB1", "#FFB74D",
        "#AB47BC", "#26C6DA", "#EF5350", "#66BB6A", "#FFA726",
        "#7E57C2", "#29B6F6", "#EC407A", "#26A69A", "#D4E157"
    ];

    public override async Task OnNavigatedToAsync()
    {
        IsBusy = true;
        await LoadAsync();
        IsBusy = false;
    }

    private async Task LoadAsync()
    {
        var list = await categoryRepository.GetAllAsync();
        Categories = new ObservableCollection<Category>(list.OrderBy(c => c.Name));
    }

    [RelayCommand]
    private void NewCategory()
    {
        SelectedCategory = null;
        FormTitle = "Nueva Categoría";
        EditName = string.Empty;
        EditDescription = string.Empty;
        EditColor = "#E8A87C";
        EditIsActive = true;
        IsEditing = true;
    }

    [RelayCommand]
    private void EditCategory(Category c)
    {
        SelectedCategory = c;
        FormTitle = "Editar Categoría";
        EditName = c.Name;
        EditDescription = c.Description;
        EditColor = string.IsNullOrEmpty(c.Color) ? "#E8A87C" : c.Color;
        EditIsActive = c.IsActive;
        IsEditing = true;
    }

    [RelayCommand]
    private void SelectColor(string hex) => EditColor = hex;

    [RelayCommand]
    private void CancelEdit() { IsEditing = false; SelectedCategory = null; }

    [RelayCommand]
    private async Task SaveCategoryAsync()
    {
        if (string.IsNullOrWhiteSpace(EditName)) { StatusMessage = "El nombre es requerido."; return; }

        var cat = SelectedCategory ?? new Category();
        cat.Name = EditName;
        cat.Description = EditDescription;
        cat.Color = EditColor;
        cat.IsActive = EditIsActive;

        IsBusy = true;
        if (cat.Id == 0) await categoryRepository.AddAsync(cat);
        else await categoryRepository.UpdateAsync(cat);

        var userId = authService.CurrentSession?.Id ?? 0;
        await logService.CategorySaved(cat.Name, userId);

        IsBusy = false;
        StatusMessage = $"Categoría '{cat.Name}' guardada.";
        IsEditing = false;
        SelectedCategory = null;
        await LoadAsync();
    }

    [RelayCommand]
    private async Task DeleteCategoryAsync(Category c)
    {
        c.IsActive = false;
        await categoryRepository.UpdateAsync(c);
        await LoadAsync();
    }
}
