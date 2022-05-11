#!/bin/bash

sed -i "s/TG_TOKEN/$TG_TOKEN/" /app/PersonalityQuizTelegram.dll.config

mv /quiztmp/*.json /app/quiz/

./PersonalityQuizTelegram