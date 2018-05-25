from django import forms
from mdeditor.fields import MDTextFormField

from post.models import Article


class AttributeForm(forms.ModelForm):
    content = MDTextFormField()

    class Meta:
        model = Article
        fields = ('title', 'cate', 'content', 'thumbnail')
