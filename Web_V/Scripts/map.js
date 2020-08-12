anychart.onDocumentReady(function () {
    
    let data_source = JSON.parse(localStorage.getItem('array'));
        // рисует карту
    var map = anychart.map();
        map.geoData('anychart.maps.world')
            .padding(0)
        ;

        map.background().fill({
            keys: [ "#dffffc","#ddfff8", "#f2fff8"],
            angle: 130,
        });

        map.unboundRegions()
            .enabled(true)
            //.fill('#f0f4ed')
            .fill({
                keys: ["#d7dbd5", "#d0d4d4", "#e5eadf"],
                angle: 130,
            })
            .stroke('#3D3C3C');

        
        map.credits()
            .enabled(true)
            .text('Data source: https://opendata.socrata.com')
            .logoSrc('https://opendata.socrata.com/stylesheets/images/common/favicon.ico');

        // Заголовок
        map.title()
            .enabled(true);
           // .padding([20, 0, 10, 0])
            //.text('Карта');
        var title = map.title();
        title.useHtml(true);
        title.text(
            "<br><a style=\"color:#000; font-size: 20px;\">"+
            "Группы"
        );

        //API читает долготу по ключевому слову long, но его невозможно передать из шарпа через json(т.к. это ключевое слово), поэтому передаю long_ а ниже переписываю в long
        var data = [];
        let lenght = data_source.length;
        for(let i = 0; i<lenght; i++){
            var temp = data_source[i].Temp;
            var number_station = data_source[i].number_station;
            var Tc = data_source[i].Tc;
            var number_group = data_source[i].number_group;
            var lat = data_source[i].lat;
            var long = data_source[i].long_;
            data.push({Temp: temp, number_station: number_station, Tc: Tc, number_group: number_group, lat: lat, long: long},);
        }
        // создает набор данных из данных образца
        var crashesDataSet = anychart.data.set(data).mapAs();

//---------------------------------------------Функции-------------------------------------------------
        // функция для создания маркера
        var createSeries = function (name, data, color) {
            var series = map.marker(data);
            series.name(name)
                .fill(color)
                .stroke(color)
                .type('circle')
                .size(4)
                .labels(false)
                .selectionMode('none')
                .tooltip({title: false, separator: false});
            series.hovered()
                .stroke(color)
                .size(8);
            series.legendItem()
                .iconType('circle')
                .iconSize(14)
                .iconFill(color)
                .iconStroke(color);
        };

        //Меняем маркеры по запросу
        window.marker_update = function (name,color, type, number,size){
            var seridddes = map.getSeriesAt(number);
            seridddes.name(name)
                .stroke(color)
                .type(type)
                .size(size)
                .fill(color);
            seridddes.hovered()
                .size(size);
            seridddes.legendItem()
                .iconFill(color)
                .iconType(type);
        };
//---------------------------------------------------------------------------------
        //Цвета маркеров
    let color = ["#000080", "#990066", "#FF6E4A", "#B8B428",
        "#3C18FF", "#FFDAB9", "#066", "#990000",
        "#F13A13", "#57ca24", "#92000A", "#346e24",
        "#EE9374", "#ACB78E", "#712F26", "#1CA9C9",
        "#FFA000", "#FF0000", "#000147", "#FF5349",
        "#FEFE22", "#025669", "#00FF00", "#534B4F",
        "#7F180D", "#00A86B", "#999950", "#BAACC7",
        "#31372B", "#003366", "#FF9218", "#FF496C",
        "#F5DEB3", "#F3DA0B", "#B7410E", "#B76E79",
        "#99FF99", "#846A20", "#BBBBBB",
        "#966A57", "#84C3BE", "#382C1E", "#B85D43",
        "#413D51", "#CADABA", "#317F43", "#8A2BE2",
        "#282828", "#6699CC", "#FF6E4A", "#7BA05B",
        "#714B23", "#CF3476", "#3B83BD", "#D8A903",
        "#472A3F", "#915F6D", "#34C924",
        "#CC6C5C", "#313830", "#310062", "#9B2F1F",
        "#C37629", "#03C03C", "#5B1E31", "#564042",
        "#371F1C", "#2B2517", "#82898F", "#CC4E5C",
        "#BA55D3", "#2B2517", "#82898F", "#CC4E5C",
        "#A12312", "#5DA130", "#45CEA2", "#FF7518",
        "#B57281", "#8A3324", "#48D1CC", "#5E490F",
        "#7D512D", "#D79D41", "#30626B",
        "#D35339", "#8C4566", "#423C63", "#EA8DF7",
        "#F75394", "#123524", "#BEF574", "#806B2A",
        "#4D7198", "#123524", "#4E1609", "#FFA474",
        "#008CF0", "#78A2B7", "#FFF8DC", "#FFCC00",
        "#2e3b4b", "#EBC2AF", "#A08040", "#7FFF00",
        "#D2691E", "#CDB891", "#45322E", "#40826D",
        "#FF845C", "#93AA00", "#00836E",
        "#08E8DE", "#FFB300", "#007CAD", "#CD00CD",
        "#99c5cc", "#f0ff83", "#1a5478", "#9a5aff",
    ];

        // прохожусь столько раз сколько номеров групп
        let json_lenght =data[data.length - 1].number_group;
        for (let i=0; i<json_lenght; i++){
            createSeries(i+1, crashesDataSet.filter('number_group', filter_function(i+1)), color[i]);
        }
        //передаю максимальное значение номера группы
    document.getElementById('number').max = json_lenght;

        map.tooltip()
            .useHtml(true)
            .padding([8, 13, 10, 13])
            .width(350)
            .fontSize(12)
            .fontColor('#e6e6e6')
            .format(function () {
                + this.getData('summary');
                //if (this.getData('summary') == 'null') summary = '';
                return '<span style="font-size: 15px">' +
                    '<span style="color: #bfbfbf">Температура: '+ '</span>' + this.getData('Temp') + '<br/>' +
                    '<span style="color: #bfbfbf">№Станции: ' + '</span>' + this.getData('number_station') + '<br/>' +
                    '<span style="color: #bfbfbf">Тс: ' + '</span>' + this.getData('Tc') + '<br/>' +
                    '<span style="color: #bfbfbf">№Группы: ' + '</span>' + this.getData('number_group') +'</span>';
            });

        // Включает легенду
        map.legend(true);
        // масштабирование
        var zoomController = anychart.ui.zoom();
        zoomController.render(map);
        // масштабирование колесиком мыши
        // map.interactivity().zoomOnMouseWheel(true);
        // sets container id for the chart
        map.container('container');
        // Рисует
    map.draw();
});

//открываю настройки маркера
function openbox() {
    let display = document.getElementById('box').style.display;
    if(display === "none") {
        document.getElementById('box').style.display = 'block';
    }
    else {
        document.getElementById('box').style.display = "none";
    }
}
//меняю маркер
function update(){
    let size = document.getElementById('size').value;
    let number = document.getElementById('number').value;
    let name = document.getElementById('name').value;
    let color = document.getElementById('color').value;

    let type = document.getElementsByName('type');
    for (var i=0;i<type.length; i++) {
        if (type[i].checked) {
            marker_update(name, color,type[i].value, number-1, size);
        }
    }
}

function filter_function(val1) {
    return function (fieldVal) {
        return val1 == fieldVal;
    };
}