from django.conf import settings
from django.conf.urls import url, include
from django.conf.urls.static import static
from django.contrib import admin
from django.contrib.staticfiles.urls import staticfiles_urlpatterns
from rest_framework import routers

from eval.views import ScaleViewSet, ScaleItemViewSet, ScaleOptionViewSet, ScaleConclusionViewSet, ScaleRecordViewSet, \
    ScaleResultViewSet
from post.views import PostViewSet, CategoryViewSet
from user.views import UserViewSet, GroupViewSet

router = routers.DefaultRouter()
router.register(r'users', UserViewSet)
router.register(r'users-group', GroupViewSet)
router.register(r'posts', PostViewSet)
router.register(r'posts-cate', CategoryViewSet)
router.register(r'eval', ScaleViewSet)
router.register(r'eval-item', ScaleItemViewSet)
router.register(r'eval-option', ScaleOptionViewSet)
router.register(r'eval-result', ScaleResultViewSet)
router.register(r'eval-conclusion', ScaleConclusionViewSet)
router.register(r'eval-record', ScaleRecordViewSet)

# Wire up our API using automatic URL routing.
# Additionally, we include login URLs for the browsable API.
urlpatterns = [
    url(r'^api/', include(router.urls)),
    # url(r'^posts/', views.PostListAPIView.as_view()),
    # url(r'^posts/(\d+)', views.PostDetailAPIView.as_view(), name='article-detail'),
    url(r'^api/comments/', include('django_comments_xtd.urls')),
    url(r'^api/api-auth/', include('rest_framework.urls', namespace='rest_framework')),
    url(r'^admin/', admin.site.urls),
]
# + static(settings.STATIC_URL, document_root=settings.STATIC_ROOT)
urlpatterns += staticfiles_urlpatterns()
urlpatterns += static(settings.MEDIA_URL, document_root=settings.MEDIA_ROOT)
