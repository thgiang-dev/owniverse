"""Executable RabbitMQ infrastructure check; no Job consumption."""

import argparse
import logging

import pika

from owniverse_worker.messaging import (
    RabbitMqConfigurationError,
    check_connection,
    connection_parameters,
)


def main() -> int:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--check-rabbitmq", action="store_true",
                        help="Check authenticated connectivity and exit (also the default).")
    parser.parse_args()
    logging.basicConfig(level=logging.INFO, format="%(levelname)s %(message)s")
    # Client exceptions/logs may include connection parameters; emit sanitized results.
    logging.getLogger("pika").setLevel(logging.CRITICAL)
    logger = logging.getLogger(__name__)
    try:
        parameters = connection_parameters()
    except RabbitMqConfigurationError as error:
        logger.error("%s", error)
        return 2
    try:
        check_connection(parameters)
    except (pika.exceptions.AMQPError, OSError, TimeoutError):
        logger.error("RabbitMQ connection failed; check broker availability, credentials and virtual host.")
        return 1
    logger.info("RabbitMQ authenticated connection check passed.")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
