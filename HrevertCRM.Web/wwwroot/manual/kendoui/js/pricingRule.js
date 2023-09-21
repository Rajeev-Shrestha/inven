$("#pricingRule").kendoTreeView({
    dragAndDrop: true,
    dataSource: [
        {
            text: "Main Rule", expanded: true, items: [
              { text: "Sub Rule" },
              { text: "Sub Rule" },
              { text: "Sub Rule" }
            ]
        },
        {
            text: "Main Rule", items: [
              { text: "Sub Rule" },
              { text: "Sub Rule" },
              { text: "Sub Rule" }
            ]
        },
        {
            text: "Main Rule", expanded: true, items: [
              { text: "Sub Rule" },
              { text: "Sub Rule" },
              { text: "Sub Rule" }
            ]
        }
    ]
});