from django.db import models

from users.models import UserProfile


class Category(models.Model):
    name = models.CharField(max_length=50, blank=False)
    updated = models.DateTimeField(auto_now=True)

    class Meta:
        ordering = ['-updated']

    def __str__(self):
        return str(self.name)


class Article(models.Model):
    title = models.CharField(max_length=100, blank=False, default='')
    cate = models.ForeignKey(Category, on_delete=models.CASCADE)
    author = models.ForeignKey(UserProfile, on_delete=models.CASCADE)
    content = models.TextField(max_length=5000, blank=False)
    thumbnail = models.ImageField(null=True, blank=True, default=None, upload_to='static/images/thumb')
    location = models.GenericIPAddressField(null=True)
    is_top = models.BooleanField('top status', default=False)
    created = models.DateTimeField(auto_now_add=True)
    updated = models.DateTimeField(auto_now=True)

    class Meta:
        ordering = ['-is_top', '-updated']

    def __str__(self):
        return str(self.title)


class Comment(models.Model):
    content = models.TextField(max_length=500, null=False, blank=False)
    parent = models.ForeignKey('self', null=True, on_delete=models.CASCADE, related_name='children')
    article = models.ForeignKey(Article, related_name='comments', on_delete=models.CASCADE)
    author = models.ForeignKey(UserProfile, on_delete=models.CASCADE)
    location = models.GenericIPAddressField(null=True)
    created = models.DateTimeField(auto_now_add=True)

    class Meta:
        ordering = ['created']

    def __str__(self):
        return str(self.content)

    def children(self):
        return Comment.objects.filter(parent=self)

    @property
    def is_parent(self):
        if self.parent is not None:
            return False
        return True