# ODF.API
Sonar: https://sonarcloud.io/summary/overall?id=Rades98_ODF.API

## Start guide
open .env file
set required passwords and ports
if you change grafana port, ensure that you've updated var/grafana/.grafana.ini file as well

### Docker-compose
run docker-compose via docker-compose up -d
to see all running containers: docker ps
to see all even not running containers: docker ps -a
for trouble shooting you can use docker logs {nameofcontainer}

to down: docker-compose down (optionaly with --remove-orphans)

### Grafana
Grafana is available on port 3000 by default 
with 
- ID: admin 
- PW: GRAFANA_PW variable in .env

to see fancy dashbod which succs at this phase:
Configuration -> Data sources -> Add data source
#### Prometheus
- Url: http://prometheus:9090

#### Elastic
##### Setting up data sources
- Url: http://elasticsearch:9200 - or ur ports specified in .env
- Auth: Basic auth
- Basic Auth Details
- - User: elastic
- - Password: pw from .env file
- Elastic details
- - Index name: odf-api-development - or production
- - Time field name: @timestamp
- - Version: 7.10+
- - Max concurrent Shard Requests: 5

##### Setting up dashboard
- Click on '+' (create)
- Import
- Upload JSON file
- insert path to /var/lib/grafana/dashboards
- select JSON file
- Import

CG! U can see my shitty dashboar now! ☺☻

                              \\\\\\\
                            \\\\\\\\\\\\
                          \\\\\\\\\\\\\\\
  -----------,-|           |C>   // )\\\\|
           ,','|          /    || ,'/////|
---------,','  |         (,    ||   /////
         ||    |          \\  ||||//''''|
         ||    |           |||||||     _|
         ||    |______      `````\____/ \
         ||    |     ,|         _/_____/ \
         ||  ,'    ,' |        /          |
         ||,'    ,'   |       |         \  |
_________|/    ,'     |      /           | |
_____________,'      ,',_____|      |    | |
             |     ,','      |      |    | |
             |   ,','    ____|_____/    /  |
             | ,','  __/ |             /   |
_____________|','   ///_/-------------/   |
              |===========,'