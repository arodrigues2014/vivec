@echo off
setlocal enabledelayedexpansion

REM Configura las variables
set DOCKERFILE_PATH=D:\Proyectos\Vrt.Vivec.Svc\Vrt.Vivec.Svc
set IMAGE_NAME=v1:latest
set AWS_REGION=eu-west-2
set AWS_ECR_REPO=v1
set AWS_ACCOUNT_ID=905418313878

REM Construye la imagen Docker
docker build -t %IMAGE_NAME% %DOCKERFILE_PATH%
if %errorlevel% neq 0 (
    echo Error: Fallo al construir la imagen Docker.
    exit /b %errorlevel%
)

REM Configura las credenciales de AWS
aws configure set aws_access_key_id AKIA5FTZDCCLNZHIRXXF
aws configure set aws_secret_access_key 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
aws configure set default.region %AWS_REGION%
aws configure set default.output json

REM Inicia sesión en Amazon ECR
aws ecr get-login-password --region %AWS_REGION% | docker login --username AWS --password-stdin %AWS_ACCOUNT_ID%.dkr.ecr.%AWS_REGION%.amazonaws.com
if %errorlevel% neq 0 (
    echo Error: Fallo al iniciar sesión en Amazon ECR.
    exit /b %errorlevel%
)

REM Etiqueta la imagen Docker
docker tag %IMAGE_NAME% %AWS_ACCOUNT_ID%.dkr.ecr.%AWS_REGION%.amazonaws.com/%AWS_ECR_REPO%

REM Sube la imagen a Amazon ECR
docker push %AWS_ACCOUNT_ID%.dkr.ecr.%AWS_REGION%.amazonaws.com/%AWS_ECR_REPO%
if %errorlevel% neq 0 (
    echo Error: Fallo al subir la imagen a Amazon ECR.
    exit /b %errorlevel%
)

echo Imagen Docker subida exitosamente a Amazon ECR.
exit /b 0