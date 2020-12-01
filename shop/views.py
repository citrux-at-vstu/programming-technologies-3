from django.shortcuts import render
from django.http import HttpResponse
from django.views.generic.edit import CreateView
from rest_framework import viewsets

from .models import Product, Purchase
from .serializers import ProductSerializer
# Create your views here.
def index(request):
    products = Product.objects.all()
    context = {'products': products}
    return render(request, 'shop/index.html', context)


class PurchaseCreate(CreateView):
    model = Purchase
    fields = ['product', 'person', 'address']

    def form_valid(self, form):
        self.object = form.save()
        return HttpResponse(f'Спасибо за покупку, {self.object.person}!')


class ProductViewSet(viewsets.ModelViewSet):
    serializer_class = ProductSerializer
    queryset = Product.objects.all()
