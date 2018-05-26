from django.contrib import admin

from post.models import Article, Category


class ArticleAdmin(admin.ModelAdmin):
    list_display = ('title', 'author', 'is_public', 'created')
    ordering = ('-created',)


# Register your models here.
admin.site.register(Article, ArticleAdmin)
admin.site.register(Category)
