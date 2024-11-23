
# MyStore Environment Setup

Este repositório contém os arquivos necessários para configurar e executar as dependências do ambiente local usando Docker Compose.

## Pré-requisitos

Certifique-se de que os seguintes softwares estão instalados na sua máquina:
- [Docker](https://docs.docker.com/get-docker/)
- [Docker Compose](https://docs.docker.com/compose/install/)
- [Bash](https://www.gnu.org/software/bash/) (para executar o script `run-env.sh`)

## Arquivos Importantes

- **`run-env.sh`**: Script para inicializar os serviços de dependência.
- **`docker-compose.dependencies.yml`**: Definições do Docker Compose para as dependências do projeto.
- **`.env.*`**: Arquivos de configuração do ambiente (desenvolvimento, produção, etc.). Certifique-se de selecionar o arquivo correto ou criar um novo com base nos existentes.

## Passos para Subir as Dependências

1. **Clone o repositório**:

   ```bash
   git clone <URL_DO_REPOSITORIO>
   cd MyStore/environment
   ```

2. **Configuração do Ambiente**:

   Certifique-se de configurar o arquivo `.env` adequado antes de subir os serviços. Por padrão, o script utiliza `.env.local`. Para usar outro arquivo, atualize o script ou exporte a variável de ambiente `ENV_FILE`:

   ```bash
   export ENV_FILE=".env.dev"
   ```

3. **Inicie as Dependências**:

   Execute o script `run-env.sh` para iniciar os serviços de dependências:

   ```bash
   bash run-env.sh
   ```

   Este script utilizará o arquivo `docker-compose.dependencies.yml` para criar e iniciar os containers.

4. **Verifique os Serviços**:

   Para garantir que os serviços foram iniciados corretamente, use o comando:

   ```bash
   docker ps
   ```

   Certifique-se de que todos os containers esperados estão em execução.

## Parando os Serviços

Para parar e remover os containers iniciados, utilize o comando abaixo:

```bash
docker-compose -f docker-compose.dependencies.yml down
```

## Logs e Depuração

Caso precise verificar os logs dos containers, utilize:

```bash
docker-compose -f docker-compose.dependencies.yml logs -f
```

## Problemas Comuns

- **Erro de Permissão**: Certifique-se de que você tem permissão para executar o Docker sem `sudo`.
- **Portas em Uso**: Verifique se as portas configuradas no `docker-compose.dependencies.yml` já estão sendo utilizadas por outro processo.
