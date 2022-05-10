FROM mcr.microsoft.com/dotnet/runtime:6.0

COPY bin/Debug/net6.0 /app
COPY quiz /quiztmp

COPY entrypoint.sh /entrypoint.sh

WORKDIR /app

ENTRYPOINT [ "/bin/bash" ]
CMD [ "/entrypoint.sh" ]