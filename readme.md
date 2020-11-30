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


### 2. Приведение теста в порядок

Теперь сделаем тест немного полезнее. Проверим, что контроллер действительно передаёт id товара в представление. Для этого преобразуем код теста к следующему виду:

```c#
   public class HomeControllerTests
    {
        [TestMethod()]
        public void BuyTest()
        {
            var controller = new HomeController();
            int id = 1;
            var result = controller.Buy(id) as ViewResult;
            Assert.AreEqual(id, result.ViewData["ProductId"]);
        }
    }
```

Попутно добавляем необходимые ссылки на сборку и недостающие директивы using.

Запускаем и видим, что тест пройден.

### 3. Моки. Библиотека NSubstitute

Суть юнит-теста в изолированном тестировании кода. Но зачастую нельзя просто так взять и изолировать функцию. В нашем примере метод контроллера `public ActionResult Index()` получает информацию из базы. В целях лучшей тестируемости было бы неплохо заменить реальную базу чем-то легко настраиваемым и предоставляющим тот же интерфейс -- моком.

Для создания моков будем использовать библиотеку NSubstitute. Установим её через Nuget.

### 4. Моки. Выделение интерфейса IShopContext

Взаимодействие с базой у нас реализовано через класс ShopContext. Только вот NSubstitute для создания мока требует интерфейс, а не класс. Поэтому выделим интерфейс IShopContext:

```c#
    public interface IShopContext
    {
        DbSet<Product> Products { get; set; }
        DbSet<Purchase> Purchases { get; set; }
        int SaveChanges();
    }
```

Определение ShopContext примет вид:

```c#
    public class ShopContext : DbContext, IShopContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
    }
```

Теперь изменим код контроллера HomeController:

```c#
    public class HomeController : Controller
    {
        private IShopContext db;
        public HomeController(IShopContext context)
        {
            db = context;
        }
        public HomeController() : this(new ShopContext()) { }
        public ActionResult Index()
        ...
```

### 4. Мокирование IShopContext

Теперь создадим свой ShopContext со свойством Products на основе списка. Для этого модифицируем код в классе теста:

```c#
    public class HomeControllerTests
    {
        private IShopContext db;

        [TestInitialize()]
        public void Setup()
        {
            db = Substitute.For<IShopContext>();

            var data = new List<Product> { new Product { ID = 1, Name = "Table", Price = 1 } }.AsQueryable();
            var products = Substitute.For<DbSet<Product>, IQueryable<Product>>();
            ((IQueryable<Product>)products).Provider.Returns(data.Provider);
            ((IQueryable<Product>)products).Expression.Returns(data.Expression);
            ((IQueryable<Product>)products).ElementType.Returns(data.ElementType);
            ((IQueryable<Product>)products).GetEnumerator().Returns(_ => data.GetEnumerator());
            products.AsNoTracking().Returns(products);

            db.Products = products;
        }
        [TestMethod()]
        public void BuyTest()
        {
            var controller = new HomeController(db);
            int id = 1;
            var result = controller.Buy(id) as ViewResult;
            Assert.AreEqual(id, result.ViewData["ProductId"]);
        }
```

Не забываем добавить нужные ссылки на сборку и директивы using.

Метод Setup будет вызываться перед выполнением теста, чтобы подготовить состояние базы. Запустим BuyTest и убедимся, что пока ничего не поломали.

Остановимся подробнее на магии, которая здесть происходит. Строчка
```c#
db = Substitute.For<IShopContext>();
```
создаёт объект-заглушку, удовлетворяющую интерфейсу IShopContext. Аналогично,
```c#
var products = Substitute.For<DbSet<Product>, IQueryable<Product>>();
```
создаёт заглушку, выглядяшую как настоящий DbSet, а страшный код после делает так, чтобы элементы хранились в списке data. Наконец, строка
```c#
db.Products = products;
```
всталяет одну заглушку в другую.

Теперь мы можем написать тест для метода Index:

```c#
        [TestMethod()]
        public void IndexTest()
        {
            var controller = new HomeController(db);
            var result = controller.Index() as ViewResult;
            CollectionAssert.AreEqual(db.Products.ToList(), ((IEnumerable<Product>)result.ViewBag.Products).ToList());
        }
```

Тест проходит, все счастливы.
