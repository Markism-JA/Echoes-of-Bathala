FROM ubuntu:22.04

RUN apt-get update && apt-get install -y ca-certificates && rm -rf /var/lib/apt/lists/*

WORKDIR /game

COPY Builds/LinuxServer/ .

EXPOSE 7777/udp

RUN chmod +x ./EchoesServer.x86_64

ENTRYPOINT ["./EchoesServer.x86_64", "-batchmode", "-nographics"]
