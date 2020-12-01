from django.urls import path
from rest_framework.routers import DefaultRouter

from . import views

router = DefaultRouter()
router.register(r'api/products', views.ProductViewSet, basename='api-product')

urlpatterns = [
    path('', views.index, name='index'),
    path('buy/<int:product_id>/', views.PurchaseCreate.as_view(), name='buy'),
    *router.urls
]