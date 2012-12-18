#!/bin/bash
pushd /opt/smartsprites-0.2.8
# --css-file-suffix=-processed doesn't seem to work any more?
# http://groups.google.com/group/smartsprites-css-sprite-generator/browse_thread/thread/be9a798d53dc9cd0
#./smartsprites.sh /home/daniel/www/dan.cx/res/sprites.css --css-file-suffix=-processed
./smartsprites.sh /home/daniel/www/dan.cx/res/sprites.css
mv /home/daniel/www/dan.cx/res/sprites-sprite.css /home/daniel/www/dan.cx/res/sprites-processed.css
popd
