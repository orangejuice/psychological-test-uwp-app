from django import forms
from mdeditor.fields import MDTextFormField

from post.models import Article


class AttributeForm(forms.ModelForm):
    content = MDTextFormField()

    def get_object(self):
        """
        Return a new (unsaved) comment object based on the information in this
        form. Assumes that the form is already validated and will throw a
        ValueError if not.

        Does not set any of the fields that would come from a Request object
        (i.e. ``user`` or ``ip_address``).
        """
        if not self.is_valid():
            raise ValueError("get_comment_object may only be called on valid forms")

        new = Article(**self.get_create_data())
        return new

    def get_create_data(self):
        return dict(
            title=self.cleaned_data["title"],
            content=self.cleaned_data["content"],
            thumbnail=self.cleaned_data['thumbnail'],
            cate=self.cleaned_data["cate"],
            is_public=False
        )

    class Meta:
        model = Article
        fields = ('title', 'cate', 'content', 'thumbnail')
