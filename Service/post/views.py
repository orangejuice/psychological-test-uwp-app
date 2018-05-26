from django.contrib.auth.decorators import login_required
from django.http import HttpResponseRedirect
from django.shortcuts import render, resolve_url
from django.urls import reverse
from django.views.decorators.csrf import csrf_protect
from django.views.decorators.http import require_POST
from django.views.generic import TemplateView, DetailView
from rest_framework import viewsets, status
from rest_framework.response import Response
from rest_framework.settings import api_settings

from post.form import AttributeForm
from post.models import Article, Category, ArticleFavorite
from post.serializers import PostListSerializer, CategorySerializer, PostDetailSerializer, ArticleFavoriteSerializer, \
    ArticleFavoriteAddSerializer


class PostViewSet(viewsets.GenericViewSet):
    queryset = Article.objects.all()

    def list(self, request):
        order = request.query_params.get('isTop')
        queryset = Article.objects.filter(is_public=1) if order is None \
            else Article.objects.filter(is_top=1, is_public=1)

        page = self.paginate_queryset(queryset)
        if page is not None:
            serializer = PostListSerializer(page, many=True, context={'request': request})
            return self.get_paginated_response(serializer.data)

        serializer = PostListSerializer(queryset, many=True, context={'request': request})
        return Response(serializer.data)

    def retrieve(self, request, pk=None):
        queryset = Article.objects.get(pk=pk, is_public=1)
        serializer = PostDetailSerializer(queryset, context={'request': request})
        return Response(serializer.data)


class CategoryViewSet(viewsets.ReadOnlyModelViewSet):
    queryset = Category.objects.all()
    serializer_class = CategorySerializer
    pagination_class = None


class PostFavoriteViewSet(viewsets.GenericViewSet):
    queryset = ArticleFavorite.objects.all()
    serializer_class = ArticleFavoriteSerializer

    def list(self, request):
        user = request.user.pk
        queryset = ArticleFavorite.objects.filter(user=user)

        page = self.paginate_queryset(queryset)
        if page is not None:
            serializer = ArticleFavoriteSerializer(page, many=True, context={'request': request})
            return self.get_paginated_response(serializer.data)

        serializer = PostListSerializer(queryset, many=True, context={'request': request})
        return Response(serializer.data)

    def create(self, request):
        post = request.data['post']
        user = request.user.pk
        request.data['user'] = request.user.pk
        qs = ArticleFavorite.objects.filter(user=user, post=post)
        if qs is not None and qs.count() > 0:
            # there are favor record already. delete it.
            qs[0].delete()
            return Response(status=status.HTTP_204_NO_CONTENT)
        serializer = ArticleFavoriteAddSerializer(data=request.data, context={'request': request})
        serializer.is_valid(raise_exception=True)
        serializer.save()
        headers = self.get_success_headers(serializer.data)
        return Response(serializer.data, status=status.HTTP_201_CREATED, headers=headers)

    @staticmethod
    def get_success_headers(data):
        try:
            return {'Location': str(data[api_settings.URL_FIELD_NAME])}
        except (TypeError, KeyError):
            return {}


class PostView(DetailView):
    model = Article
    template_name = 'post/comment.html'

    def get_context_data(self, **kwargs):
        context = super(PostView, self).get_context_data(**kwargs)
        # context.update({'next': reverse('comments-xtd-sent')})
        context.update({'next': reverse('post_comment', args=[kwargs['object'].pk])})
        return context


@csrf_protect
@require_POST
@login_required
def post_new(request):
    data = request.POST.copy()
    form = AttributeForm(data)
    if form.errors:
        return render(request, 'post/new.html', {
            "content": form.data.get("content", ""),
            "form": form
        })
    if form.is_valid():
        model = form.get_object()
        model.author = request.user
        model.ip_address = request.META.get("REMOTE_ADDR", None)
        model.save()
        return HttpResponseRedirect(resolve_url('post_success'))


def post_success(request):
    return render(request, 'post/done.html')


class PostEditorView(TemplateView):
    template_name = 'post/new.html'
    form = AttributeForm()

    def get_context_data(self, **kwargs):
        context = {'form': self.form, 'title': 'Post Form'}
        return context
