from django.test import TestCase
from django.urls import reverse
from .models import Product

# Create your tests here.
class IndexViewTests(TestCase):
    def test_no_products(self):
        response = self.client.get(reverse('index'))
        self.assertEqual(response.status_code, 200)
        self.assertQuerysetEqual(response.context['products'], [])
