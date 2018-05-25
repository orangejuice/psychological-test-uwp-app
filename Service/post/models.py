from django.db import models
from django.urls import reverse

from user.models import UserProfile


class Category(models.Model):
    name = models.CharField(max_length=50, blank=False)
    created = models.DateTimeField(auto_now=True)

    class Meta:
        ordering = ['created']

    def __str__(self):
        return str(self.name)


class Article(models.Model):
    title = models.CharField(max_length=100, blank=False, default='')
    cate = models.ForeignKey(Category, on_delete=models.CASCADE)
    author = models.ForeignKey(UserProfile, on_delete=models.DO_NOTHING)
    content = models.TextField(max_length=5000, blank=False, null=False)
    thumbnail = models.ImageField(null=True, blank=True, default='thumb/default.jpg', upload_to='thumb')
    ip_address = models.GenericIPAddressField(unpack_ipv4=True, blank=True, null=True)
    is_top = models.BooleanField('置顶', default=False)
    allow_comments = models.BooleanField('允许评论', default=True)
    created = models.DateTimeField(auto_now_add=True)
    updated = models.DateTimeField(auto_now=True)

    class Meta:
        ordering = ['-created']

    def __str__(self):
        return str(self.title)

    def get_absolute_url(self):
        return reverse('post_comment', args=[self.pk])


class ArticleFavorite(models.Model):
    # 一个问题 -> 多个答案， 不同程度的对应不同的结果
    post = models.ForeignKey(to=Article, on_delete=models.CASCADE)
    user = models.ForeignKey(to=UserProfile, on_delete=models.CASCADE)
    created = models.DateTimeField(auto_now_add=True)

    class Meta:
        ordering = ['-created']
        db_table = 'post_favor'
