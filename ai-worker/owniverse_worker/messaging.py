"""RabbitMQ connectivity only; no message contracts or consumers."""

import os
from urllib.parse import urlsplit

import pika


class RabbitMqConfigurationError(ValueError):
    """Invalid local configuration, with a credential-free error message."""


def connection_parameters() -> pika.ConnectionParameters:
    value = os.environ.get("RABBITMQ_URL", "")
    try:
        uri = urlsplit(value)
        if (
            not value or any(character.isspace() for character in value)
            or uri.scheme not in ("amqp", "amqps") or not uri.hostname
            or not uri.username or not uri.password or uri.port == 0
            or uri.query or uri.fragment or "/" in uri.path[1:]
        ):
            raise ValueError()
        parameters = pika.URLParameters(value)
        # Match the .NET client's default virtual host for an omitted/empty path.
        if uri.path in ("", "/"):
            parameters.virtual_host = "/"
        parameters.connection_attempts = 1
        parameters.socket_timeout = 5
        parameters.stack_timeout = 6
        parameters.blocked_connection_timeout = 5
        return parameters
    except (ValueError, TypeError):
        raise RabbitMqConfigurationError(
            "RABBITMQ_URL is required and must be an amqp/amqps URI with host, "
            "username and password, and without query or fragment."
        ) from None


def check_connection(parameters: pika.ConnectionParameters) -> None:
    connection = pika.BlockingConnection(parameters)
    try:
        channel = connection.channel()
        channel.close()
    finally:
        if connection.is_open:
            connection.close()
