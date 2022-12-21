```curl
curl -v --request OPTIONS **'localhost:5001'** --header 'Origin: **http://localhost:60001**'; --header 'Access-Control-Request-Method: GET'
```

curl -v --request OPTIONS 'localhost:5001' --header 'Origin: http://localhost:4001'; --header 'Access-Control-Request-Method: GET'
