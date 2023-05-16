FROM python:3.11

WORKDIR /code

COPY ./Pipfile /code/Pipfile
COPY ./Pipfile.lock /code/Pipfile.lock

RUN pip install pipenv

RUN python -m pipenv install

RUN pip install uvicorn

COPY ./.env /code/.env
COPY ./app /code/app

CMD [ "python", "-m", "uvicorn", "app.server:app", "--host", "0.0.0.0", "--port", "3000" ]