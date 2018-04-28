from rest_framework import serializers

from eval.models import Scale


class ScaleListSerializer(serializers.HyperlinkedModelSerializer):
    class Meta:
        model = Scale
        fields = ('url', 'title', 'introduction', 'thumbnail', 'created', 'is_top')

# class ScaleItemSerializer(serializers.HyperlinkedModelSerializer):
#     class Meta:
#         model = ScaleItem
#         fields = ()
