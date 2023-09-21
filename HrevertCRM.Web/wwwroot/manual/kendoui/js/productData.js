var dataSource = new kendo.data.DataSource({
    transport: {
        read: {
            url: "/api/product",
            dataType: "json"
        },
        update: {
            url: "/api/product",
            type: "put",
            dataType: "json"
        },
        destroy: {
            url: "/api/product",
            dataType: "json"
        },
        create: {
            url: "/api/product",
            type: "post",
            dataType: "json"
        },
        parameterMap: function (options, operation) {
            if (operation !== "read" && options.models) {
                return { models: kendo.stringify(options.models) };
            }
        }
    },
    batch: false,
    pageSize: 20,
    schema: {
        model: {
            id: "id",
            fields: {
                id: { editable: false, nullable: true },
                code: { type: "string" },
                name: { type: "string" },
                unitPrice: { type: "number" },
                quantityOnHand: { type: "number" },
                quantityOnOrder: { type: "number" },
                productDescription: { type: "string" },
                categories: { type: "number" }
            }
        }
    }
});

$("#grid").kendoGrid({
    dataSource: dataSource,
    pageable: {
        refresh: true
    },
    height: 450,
    toolbar: ["create"],
    columns: [
        "id",
        "code",
        "name",
        "unitPrice",
        "quantityOnHand",
        "quantityOnOrder",
        "productDescription",
        "categories",

        //{ id: "id", title: "ID", width: "120px" },
        //{ name: "name", title: "Name", width: "120px" },
        //{ code: "code", title: "Code", width: "120px" },
        //{ unitPrice: "unitPrice", title: "Name", width: "120px" },
        { command: ["edit", "destroy"], title: "&nbsp;", width: "250px" }
    ],
    editable: "popup"
});