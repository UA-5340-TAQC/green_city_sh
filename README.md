# green_city_sh

```
MyAutomationSolution/
├── MyProject.Tests/
│   ├── PageObjects/             # Опис сторінок
│   │   ├── BasePage.cs          # Базовий клас для всіх сторінок
│   │   ├── HomePage.cs
│   │   ├── LoginPage.cs
│   │   └── Components/          # Спільні компоненти
│   │       ├── HeaderComponent.cs
│   │       ├── FooterComponent.cs
│   │       └── TableComponent.cs
│   ├── Tests/                   # Тестові сценарії
│   │   ├── BaseTest.cs          # Setup/Teardown та драйвер
│   │   ├── LoginTests.cs
│   │   └── CheckoutTests.cs
│   ├── Models/                  # Об'єкти даних (DTO)
│   │   └── User.cs              # Клас користувача (Login, Password)
│   ├── Utils/                   # Допоміжні інструменти
│   │   ├── ConfigReader.cs      # Читання appsettings.json
│   │   ├── WaitHelpers.cs       # Налаштовані очікування
│   │   └── ScreenshotMaker.cs   # Скріншоти при падіннях
│   ├── Data/                    # Тестові дані
│   │   └── testdata.json
│   ├── appsettings.json         # Глобальні налаштування (URL, Browser)
│   └── MyProject.Tests.csproj   # Файл проекту з NuGet залежностями
```
