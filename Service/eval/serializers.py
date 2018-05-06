from rest_framework import serializers

from eval.models import Scale, ScaleItem, ScaleOption, ScaleConclusion, ScaleResult, ScaleRecord, ScaleItemOpt


class ScaleListSerializer(serializers.HyperlinkedModelSerializer):
    done = serializers.SerializerMethodField()

    class Meta:
        model = Scale
        fields = ('url', 'title', 'introduction', 'thumbnail', 'created', 'is_top', 'done')

    def get_done(self, obj):
        user = self.context['request'].user.pk
        qs = ScaleRecord.objects.filter(scale=obj.pk, user=user)[:1]
        record = ScaleRecordSerializer(qs, many=True, context=self.context).data
        return record[0]['url'] if qs.count() else None


class ScaleOptionSerializer(serializers.ModelSerializer):
    class Meta:
        model = ScaleOption
        fields = ('key', 'value')


class ScaleResultSerializer(serializers.ModelSerializer):
    class Meta:
        model = ScaleResult
        fields = ('key', 'value')


class ScaleItemOptSerializer(serializers.ModelSerializer):
    key = serializers.ReadOnlyField(source='option.key')
    value = serializers.ReadOnlyField(source='option.value')
    score = serializers.ReadOnlyField(source='option.score')
    bonus = ScaleResultSerializer(read_only=True)

    class Meta:
        model = ScaleItemOpt
        fields = ('key', 'value', 'bonus', 'score')


class ScaleItemSerializer(serializers.HyperlinkedModelSerializer):
    opts = ScaleOptionSerializer(many=True, read_only=True)

    class Meta:
        model = ScaleItem
        fields = ('url', 'sn', 'question', 'opts')


class ScaleItemCalSerializer(serializers.ModelSerializer):
    opts = ScaleItemOptSerializer(many=True, source='scaleitemopt_set', read_only=True)

    class Meta:
        model = ScaleItem
        fields = ('sn', 'question', 'opts')


class ScaleSerializer(serializers.HyperlinkedModelSerializer):
    items = serializers.SerializerMethodField()

    class Meta:
        model = Scale
        fields = ('url', 'title', 'items')

    def get_items(self, obj):
        qs = ScaleItem.objects.filter(scale=obj.pk)
        items = ScaleItemSerializer(qs, many=True, context=self.context).data
        return items


class ScaleConclusionSerializer(serializers.HyperlinkedModelSerializer):
    class Meta:
        model = ScaleConclusion
        fields = ('url', 'key', 'description')


class ScaleRecordSerializer(serializers.HyperlinkedModelSerializer):
    conclusion = serializers.SerializerMethodField()

    class Meta:
        model = ScaleRecord
        fields = ('url', 'score', 'conclusion', 'created')

    def get_conclusion(self, obj):
        # 分别得分 + 代码对应结论
        # HOLLAND   分别得分 + 1代码对应结论 + 3代码对应结论
        con = {}
        if obj.scale.pk is 2:
            qs = ScaleConclusion.objects.get(scale=obj.scale, key=obj.result[0])
            con[0] = ScaleConclusionSerializer(qs, context=self.context).data
            qs = ScaleConclusion.objects.get(scale=obj.scale, key=obj.result)
            con[1] = ScaleConclusionSerializer(qs, context=self.context).data
        else:
            qs = ScaleConclusion.objects.get(scale=obj.scale, key=obj.result)
            con[0] = ScaleConclusionSerializer(qs, context=self.context).data
        return con


class ScaleRecordAddSerializer(serializers.ModelSerializer):
    class Meta:
        model = ScaleRecord
        fields = ('user', 'scale', 'chose', 'score', 'result')
