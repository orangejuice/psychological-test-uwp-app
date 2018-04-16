from django.conf.urls import url, include
from django.contrib import admin
from rest_framework import routers

from posts.views import PostViewSet, CategoryViewSet, CommentViewSet
from users.views import UserViewSet, GroupViewSet

router = routers.DefaultRouter()
router.register(r'users', UserViewSet)
router.register(r'users-group', GroupViewSet)
router.register(r'posts', PostViewSet)
router.register(r'posts-cate', CategoryViewSet)
router.register(r'posts-comment', CommentViewSet)

# Wire up our API using automatic URL routing.
# Additionally, we include login URLs for the browsable API.
urlpatterns = [
    url(r'^', include(router.urls)),
    url(r'^posts/comments/', include('django_comments.urls')),
    url(r'^api-auth/', include('rest_framework.urls', namespace='rest_framework')),
    url(r'^admin/', admin.site.urls),
]