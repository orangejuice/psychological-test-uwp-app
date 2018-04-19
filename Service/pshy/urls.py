from django.conf import settings
from django.conf.urls import url, include
from django.conf.urls.static import static
from django.contrib import admin
from django.contrib.staticfiles.urls import staticfiles_urlpatterns
from rest_framework import routers

# from posts.views import PostViewSet, CategoryViewSet
from posts.views import PostViewSet, CategoryViewSet
from users.views import UserViewSet, GroupViewSet

router = routers.DefaultRouter()
router.register(r'users', UserViewSet)
router.register(r'users-group', GroupViewSet)
router.register(r'posts', PostViewSet)
router.register(r'posts-cate', CategoryViewSet)

# Wire up our API using automatic URL routing.
# Additionally, we include login URLs for the browsable API.
urlpatterns = [
    url(r'^', include(router.urls)),
    # url(r'^posts/', views.PostListAPIView.as_view()),
    # url(r'^posts/(\d+)', views.PostDetailAPIView.as_view(), name='article-detail'),
    url(r'^comments/', include('django_comments_xtd.urls')),
    url(r'^api-auth/', include('rest_framework.urls', namespace='rest_framework')),
    url(r'^admin/', admin.site.urls),
]
# + static(settings.STATIC_URL, document_root=settings.STATIC_ROOT)
urlpatterns += staticfiles_urlpatterns()
urlpatterns += static(settings.MEDIA_URL, document_root=settings.MEDIA_ROOT)
