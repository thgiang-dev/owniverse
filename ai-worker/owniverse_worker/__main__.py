"""Executable startup smoke check for the worker skeleton."""

import logging


def main() -> None:
    """Confirm startup and exit; job consumption is added in a later task."""
    logging.basicConfig(level=logging.INFO, format="%(levelname)s %(message)s")
    logging.getLogger(__name__).info("OWNIVERSE worker skeleton started.")


if __name__ == "__main__":
    main()
