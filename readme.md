## Создание юнит-тестов для приложения на Django

### 1. Создание модульного теста

При создании приложения shop в лабораторной работе № 2 django среди прочих сгенерировала файл `shop/tests.py` со следующим содержимым:

```python
from django.test import TestCase

# Create your tests here.
```

Именно в нём предполагается размещать тесты. Давайте напишем простейший тест:

```python
from django.test import TestCase
from django.urls import reverse
from .models import Product

# Create your tests here.
class IndexViewTests(TestCase):
    def test_no_products(self):
        response = self.client.get(reverse('index'))
        self.assertEqual(response.status_code, 200)
        self.assertQuerysetEqual(response.context['products'], [])
```

Тесты организуются в виде методов класса-наследника TestCase. django.test представляет интерфейс для написания тестов, совместимый с модулем стандартной библиотеки unittest и расширяющий его. В представленном тесте мы проверяем, что если при пустой базе совершить запрос к url /, помеченному у нас как index в urls.py, то код ответа будет 200 и контекстная переменная products будет пустым queryset'ом.

Для запуска тестов используется manage.py:
```bash
python manage.py test
```

После запуска мы увидим следующее:
```bash
Creating test database for alias 'default'...
System check identified no issues (0 silenced).
.
----------------------------------------------------------------------
Ran 1 test in 0.012s

OK
Destroying test database for alias 'default'...
```

Как мы видим, тест прошёл успешно. Ещё можно заметить, что django создал тестовую базу данных, запустил тест и затем удалил её.
