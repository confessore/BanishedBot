#!/bin/sh

sudo systemctl stop banishedbot.service

sudo systemctl disable banishedbot.service

sudo rm /etc/systemd/system/banishedbot.service
