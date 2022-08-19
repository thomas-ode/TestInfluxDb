docker pull influxdb:2.3.0
docker pull grafana/grafana
docker run -d --name=grafana -p 3000:3000 grafana/grafana
docker run --name influxdb -p 8086:8086 influxdb:2.3.0
