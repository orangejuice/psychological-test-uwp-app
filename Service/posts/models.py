from django.contrib.auth.models import User
from django.db import models


class Category(models.Model):
    name = models.CharField(max_length=50, blank=False)
    updated = models.DateTimeField(auto_now=True)

    class Meta:
        ordering = ['-updated']

    def __str__(self):
        return str(self.name)


# Create your models here.
class Article(models.Model):
    title = models.CharField(max_length=100, blank=False, default='')
    cate = models.ForeignKey(Category, related_name='cate_name', on_delete=models.CASCADE)
    author = models.ForeignKey(User, on_delete=models.CASCADE)
    content = models.TextField(max_length=5000, blank=False)
    thumbnail = models.ImageField(null=True, blank=True, default=None, upload_to='static/images/thumb')
    is_top = models.BooleanField('top status', default=False)
    created = models.DateTimeField(auto_now_add=True)
    updated = models.DateTimeField(auto_now=True)

    class Meta:
        ordering = ['-is_top', '-updated']

    def __str__(self):
        return str(self.title)
