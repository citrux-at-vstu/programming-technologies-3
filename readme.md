## Создание юнит-тестов для приложения на ASP.NET (C#)

### 1. Создание юнит-теста

Создадим модульный тест для метода `public ActionResult Buy(int id)` контроллера `HomeController`. Для этого шёлкнем правой кнопкой по сигнатуре метода и выберем пункт меню создание модульных тестов. Во всплывающем окне оставим всё как есть и нажмём ок.

Visual Studio сгенерирует нма новый проект exampleTests и разместит заготовку теста         `public void BuyTest()` в классе `HomeControllersTests`:

```c#
namespace example.Controllers.Tests
{
    [TestClass()]
    public class HomeControllerTests
    {
        [TestMethod()]
        public void BuyTest()
        {
            Assert.Fail();
        }
    }
}
```

Данный тест можно запустить как из меню `Тест` или обозревателя тестов, так и нажав левой кнопкой мыши по пиктограмме рядом с ео объявлением в окне редактора кода. Запустим его и убедимся, что он не проходит.
