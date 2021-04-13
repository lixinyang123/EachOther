docker pull registry.cn-shenzhen.aliyuncs.com/lllxy/eachother:latest
docker tag registry.cn-shenzhen.aliyuncs.com/lllxy/eachother:latest lixinyang/eachother:latest
docker rmi registry.cn-shenzhen.aliyuncs.com/lllxy/eachother:latest
docker images  | grep none | awk '{print $3}' | xargs docker rmi
