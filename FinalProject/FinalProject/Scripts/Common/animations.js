var animations = {
    "Template 1": function animate(canvas) {
        (function innerFunction() {
            canvas.forEachObject(function (obj) {
                if (obj.type == "text") {
                    obj.angle += 1;
                }
            });
            canvas.renderAll();
            fabric.util.requestAnimFrame(innerFunction);
        })();
    },
    "Template 2": function animate(canvas) {
        (function innerFunction() {
            canvas.forEachObject(function (obj) {
                if (obj.type == "text") {
                    obj.left = (obj.left + 1) % (500 - obj.width);
                }
            });
            canvas.renderAll();
            fabric.util.requestAnimFrame(innerFunction);
        })();
    },
    "Template 3": function animate(canvas) {
        (function innerFunction() {
            canvas.forEachObject(function (obj) {
                if (obj.type == "text") {
                    obj.fontSize = (obj.fontSize + 0.5) % 40;
                }
            });
            canvas.renderAll();
            fabric.util.requestAnimFrame(innerFunction);
        })();
    },
};