#!/bin/bash

# Verifica se os parâmetros corretos foram passados
if [ "$#" -lt 2 ]; then
  echo "Uso: ./run-env.sh <ambiente> <comando> [serviço]"
  echo "Exemplo: ./run-env.sh docker up dependencies"
  exit 1
fi

# Parâmetros
ENV_FILE="environment/.env.$1"
COMPOSE_FILE="environment/docker-compose.$3.yml"
COMMAND=$2

# Verifica se o arquivo .env existe
if [ -f "$ENV_FILE" ]; then
  echo "Usando arquivo de ambiente: $ENV_FILE"
else
  echo "Arquivo $ENV_FILE não encontrado."
  exit 1
fi

# Executa o comando especificado
case $COMMAND in
  up)
    docker compose --env-file $ENV_FILE -f $COMPOSE_FILE up -d
    ;;
  start)
    docker compose --env-file $ENV_FILE -f $COMPOSE_FILE start
    ;;
  stop)
    docker compose --env-file $ENV_FILE -f $COMPOSE_FILE stop
    ;;
  rm)
    docker compose --env-file $ENV_FILE -f $COMPOSE_FILE down
    ;;
  *)
    echo "Comando inválido. Use up, start, stop ou rm."
    ;;
esac
