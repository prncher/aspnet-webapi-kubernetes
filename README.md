Dockerfile
=============
# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

# Copy the .csproj file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the source code
COPY . .

# Publish the application
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Create the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

# Copy the published application from the build stage
COPY --from=build /app/publish .

# Expose the port your application listens on (e.g., 80 for HTTP)
EXPOSE 8080

# Define the entry point for the application
ENTRYPOINT ["dotnet", "TestWebApi.dll"]

docker build -t webapiapp . (Here webapiapp is the image name)
docker run -it --rm -p 4000:8080 --name webapiapp-container webapiapp (aspnetcore-sample is the container name)

1. minikube docker-env
2. @FOR /f "tokens=*" %i IN ('minikube -p minikube docker-env --shell cmd') DO @%I
3. minikube start --hyperv-use-external-switch --driver=docker --docker-env=local
4. minikube -p minikube docker-env
5. minikube addons enable metrics-server
6. docker build -t webapiapp .
7. notepad deployment.yaml


   apiVersion: apps/v1
    kind: Deployment
    metadata:
      name: webapiapp-app-deployment
    spec:
      replicas: 1
      selector:
        matchLabels:
          app: webapiapp-app
      template:
        metadata:
          labels:
            app: webapiapp-app
        spec:
          containers:
          - name: webapiapp-container
            image: webapiapp
            imagePullPolicy: Never # Crucial for local images
            ports:
            - containerPort: 8080 # Replace with your application's port


8. notepad service.yaml

    apiVersion: v1
    kind: Service
    metadata:
      name: webapiapp-app-service
    spec:
      selector:
        app: webapiapp-app
      ports:
        - protocol: TCP
          port: 30080
          targetPort: 8080 # Replace with your application's port
      type: NodePort # Or ClusterIP, LoadBalancer depending on your needs

9. kubectl apply -f deployment.yaml
10. minikube kubectl -- get deployments
NAME                       READY   UP-TO-DATE   AVAILABLE   AGE
webapiapp-app-deployment   1/1     1            1           9s
11. minikube kubectl -- apply -f service.yaml
12. minikube kubectl -- get services
NAME                    TYPE        CLUSTER-IP      EXTERNAL-IP   PORT(S)           AGE
kubernetes              ClusterIP   10.96.0.1       <none>        443/TCP           3h12m
webapiapp-app-service   NodePort    10.99.204.149   <none>        30080:32653/TCP   9s

13. minikube service  webapiapp-app-service --url
Open another command prompt.
14 kubectl get service
15. minikube service webapiapp-app-service
