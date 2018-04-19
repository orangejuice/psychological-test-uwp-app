import json
from urllib.request import urlopen


class GeoNotFoundException(Exception):
    pass


def get_ip_area(ip):
    if ip is None:
        raise GeoNotFoundException
    url = "http://ip.taobao.com/service/getIpInfo.php?ip=%s" % ip
    content = urlopen(url).read()
    try:
        return json.loads(content)
    except:
        raise GeoNotFoundException

    # data = json.loads(content)['data']
    # code = json.loads(content)['code']
    # if code == 0:  # success
    #     return data
    # else:
    #     return GeoNotFoundException('未知位置')


if __name__ == '__main__':
    ip = '123.125.114.144'
    print(get_ip_area(ip))
