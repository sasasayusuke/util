#-------------------------------------------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See https://go.microsoft.com/fwlink/?linkid=2090316 for license information.
#-------------------------------------------------------------------------------------------------------------

# Update the VARIANT arg in devcontainer.json to pick a Python version: 3, 3.8, 3.7, 3.6
# To fully customize the contents of this image, use the following Dockerfile instead:
# https://github.com/microsoft/vscode-dev-containers/tree/v0.109.0/containers/python-3/.devcontainer/base.Dockerfile
ARG VARIANT=3.12
FROM mcr.microsoft.com/vscode/devcontainers/python:${VARIANT}

# [Optional] If your requirements rarely change, uncomment this section to add them to the image.
#
# COPY requirements.txt /tmp/pip-tmp/
# RUN pip3 --disable-pip-version-check --no-cache-dir install -r /tmp/pip-tmp/requirements.txt \
#    && rm -rf /tmp/pip-tmp

# [Optional] Allow the vscode user to pip install globally w/o sudo
ENV PIP_TARGET=/usr/local/pip-global
ENV PYTHONPATH=${PIP_TARGET}:${PYTHONPATH}
ENV PATH=${PIP_TARGET}/bin:${PATH}
RUN mkdir -p ${PIP_TARGET} \
  && chown vscode:root ${PIP_TARGET} \
  && export SNIPPET="if [ \"\$(stat -c '%U' ${PIP_TARGET})\" != \"vscode\" ]; then chown -R vscode:root ${PIP_TARGET}; fi" \
  && echo "$SNIPPET" | tee -a /root/.bashrc >> /home/vscode/.bashrc \
  && echo "$SNIPPET" | tee -a /root/.zshrc >> /home/vscode/.zshrc

# [Optional] Uncomment this section to install additional packages.
#
# ENV DEBIAN_FRONTEND=noninteractive
# RUN apt-get update \
#    && apt-get -y install --no-install-recommends nodejs npm \
#    #
#    # Clean up
#    && apt-get autoremove -y \
#    && apt-get clean -y \
#    && rm -rf /var/lib/apt/lists/*
# ENV DEBIAN_FRONTEND=dialog

#
# Locale
RUN apt-get install -y git procps locales \
  && sed -i '/^#.* ja_JP.UTF-8 /s/^#//' /etc/locale.gen \
  && locale-gen \
  && ln -fs /usr/share/zoneinfo/Asia/Tokyo /etc/localtime\
  && dpkg-reconfigure -f noninteractive tzdata
ENV LANG='ja_jp.utf8'

#
# Node.js
# RUN curl -sL https://deb.nodesource.com/setup_14.x | bash -
# RUN apt-get update && apt-get install -y nodejs

#
# Serverless Framework
USER 1000:1000
RUN curl -o- -L https://slss.io/install | bash

# PA証明書設定
COPY /.devcontainer/ssl/cert_IRET-SSL-TrsutCA.crt /etc/ssl/certs/cert_IRET-SSL-TrsutCA.crt
ENV CURL_CA_BUNDLE=/etc/ssl/certs/cert_IRET-SSL-TrsutCA.crt
ENV AWS_CA_BUNDLE=/etc/ssl/certs/cert_IRET-SSL-TrsutCA.crt
ENV NODE_EXTRA_CA_CERTS=/etc/ssl/certs/cert_IRET-SSL-TrsutCA.crt
ENV REQUESTS_CA_BUNDLE=/etc/ssl/certs/cert_IRET-SSL-TrsutCA.crt
ENV OPENSSL_CONF=/etc/ssl/openssl.cnf
