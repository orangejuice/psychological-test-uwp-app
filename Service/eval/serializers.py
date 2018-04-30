from rest_framework import serializers

from eval.models import Scale, ScaleItem, ScaleOption


class ScaleListSerializer(serializers.HyperlinkedModelSerializer):
    class Meta:
        model = Scale
        fields = ('url', 'title', 'introduction', 'thumbnail', 'created', 'is_top')


class ScaleOptionSerializer(serializers.HyperlinkedModelSerializer):
    class Meta:
        model = ScaleOption
        fields = ('url', 'key', 'value')


class ScaleItemSerializer(serializers.HyperlinkedModelSerializer):
    options = ScaleOptionSerializer(many=True, source='opts', read_only=True)

    # options = serializers.SerializerMethodField()

    class Meta:
        model = ScaleItem
        fields = ('url', 'sn', 'question', 'options')

    # def get_options(self, obj):
    #     qs = ScaleOption.objects.filter(scale=obj.pk)
    #     opts = ScaleItemSerializer(qs, many=True, context=self.context).data
    #     return opts


class ScaleSerializer(serializers.HyperlinkedModelSerializer):
    items = serializers.SerializerMethodField()

    class Meta:
        model = Scale
        fields = ('url', 'title', 'items')

    def get_items(self, obj):
        qs = ScaleItem.objects.filter(scale=obj.pk)
        items = ScaleItemSerializer(qs, many=True, context=self.context).data
        return items
