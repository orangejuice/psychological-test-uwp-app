from django.http import HttpResponse
from django.shortcuts import render


# Create your views here.
def index(request):
    # return HttpResponse("hello, world!")
    return render(request, "index.html")


def rec(request):
    username = request.POST['username']
    password = request.POST['password']
    print(username, password)
    return render(request, "index.html")
