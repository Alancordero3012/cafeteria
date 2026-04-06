@echo off
set DOTNET="C:\Program Files\dotnet\dotnet.exe"
set ROOT=d:\cafeteria

cd /d %ROOT%

%DOTNET% new sln -n CafeteriaSystem --force
%DOTNET% new classlib -n CafeteriaSystem.Domain -f net8.0 -o CafeteriaSystem.Domain --force
%DOTNET% new classlib -n CafeteriaSystem.Application -f net8.0 -o CafeteriaSystem.Application --force
%DOTNET% new classlib -n CafeteriaSystem.Infrastructure -f net8.0 -o CafeteriaSystem.Infrastructure --force
%DOTNET% new wpf -n CafeteriaSystem.WPF -f net8.0-windows -o CafeteriaSystem.WPF --force
%DOTNET% new xunit -n CafeteriaSystem.Tests -f net8.0 -o CafeteriaSystem.Tests --force

%DOTNET% sln add CafeteriaSystem.Domain/CafeteriaSystem.Domain.csproj
%DOTNET% sln add CafeteriaSystem.Application/CafeteriaSystem.Application.csproj
%DOTNET% sln add CafeteriaSystem.Infrastructure/CafeteriaSystem.Infrastructure.csproj
%DOTNET% sln add CafeteriaSystem.WPF/CafeteriaSystem.WPF.csproj
%DOTNET% sln add CafeteriaSystem.Tests/CafeteriaSystem.Tests.csproj

%DOTNET% add CafeteriaSystem.Application/CafeteriaSystem.Application.csproj reference CafeteriaSystem.Domain/CafeteriaSystem.Domain.csproj
%DOTNET% add CafeteriaSystem.Infrastructure/CafeteriaSystem.Infrastructure.csproj reference CafeteriaSystem.Domain/CafeteriaSystem.Domain.csproj
%DOTNET% add CafeteriaSystem.Infrastructure/CafeteriaSystem.Infrastructure.csproj reference CafeteriaSystem.Application/CafeteriaSystem.Application.csproj
%DOTNET% add CafeteriaSystem.WPF/CafeteriaSystem.WPF.csproj reference CafeteriaSystem.Domain/CafeteriaSystem.Domain.csproj
%DOTNET% add CafeteriaSystem.WPF/CafeteriaSystem.WPF.csproj reference CafeteriaSystem.Application/CafeteriaSystem.Application.csproj
%DOTNET% add CafeteriaSystem.WPF/CafeteriaSystem.WPF.csproj reference CafeteriaSystem.Infrastructure/CafeteriaSystem.Infrastructure.csproj
%DOTNET% add CafeteriaSystem.Tests/CafeteriaSystem.Tests.csproj reference CafeteriaSystem.Application/CafeteriaSystem.Application.csproj
%DOTNET% add CafeteriaSystem.Tests/CafeteriaSystem.Tests.csproj reference CafeteriaSystem.Infrastructure/CafeteriaSystem.Infrastructure.csproj

%DOTNET% add CafeteriaSystem.Application/CafeteriaSystem.Application.csproj package Microsoft.Extensions.DependencyInjection --version 8.0.1
%DOTNET% add CafeteriaSystem.Application/CafeteriaSystem.Application.csproj package QuestPDF --version 2024.10.2
%DOTNET% add CafeteriaSystem.Infrastructure/CafeteriaSystem.Infrastructure.csproj package Microsoft.Extensions.DependencyInjection --version 8.0.1
%DOTNET% add CafeteriaSystem.WPF/CafeteriaSystem.WPF.csproj package CommunityToolkit.Mvvm --version 8.3.2
%DOTNET% add CafeteriaSystem.WPF/CafeteriaSystem.WPF.csproj package MaterialDesignThemes --version 5.1.0
%DOTNET% add CafeteriaSystem.WPF/CafeteriaSystem.WPF.csproj package LiveChartsCore.SkiaSharpView.WPF --version 2.0.0-rc5.4 --prerelease
%DOTNET% add CafeteriaSystem.WPF/CafeteriaSystem.WPF.csproj package Microsoft.Extensions.Hosting --version 8.0.1
%DOTNET% add CafeteriaSystem.WPF/CafeteriaSystem.WPF.csproj package QuestPDF --version 2024.10.2
%DOTNET% add CafeteriaSystem.WPF/CafeteriaSystem.WPF.csproj package ClosedXML --version 0.102.2

echo SETUP_COMPLETE
