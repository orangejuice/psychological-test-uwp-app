from django.db import models

from users.models import UserProfile


class Category(models.Model):
    name = models.CharField(max_length=50, blank=False)
    updated = models.DateTimeField(auto_now=True)

    class Meta:
        ordering = ['updated']

    def __str__(self):
        return str(self.name)


class Article(models.Model):
    title = models.CharField(max_length=100, blank=False, default='')
    cate = models.ForeignKey(Category, on_delete=models.CASCADE)
    author = models.ForeignKey(UserProfile, on_delete=models.CASCADE)
    content = models.TextField(max_length=5000, blank=False)
    thumbnail = models.ImageField(null=True, blank=True, default=None, upload_to='static/images/thumb')
    location = models.GenericIPAddressField(null=True)
    is_top = models.BooleanField('是否置顶', default=False)
    created = models.DateTimeField(auto_now_add=True)
    updated = models.DateTimeField(auto_now=True)

    class Meta:
        ordering = ['-is_top', '-updated']

    def __str__(self):
        return str(self.title)

# class Comments(Comment):
#     article = models.ForeignKey(Article, default=None, related_name='comments', on_delete=models.CASCADE)
#
#     class Meta:
#         ordering = ('-submit_date',)
