db.Person.aggregate([
    {"$lookup": {
        from: 'Car',
        pipeline: [],
        as: 'data_Car'
    }},
    {"$lookup": {
        from: 'Supplier',
        pipeline: [],
        as: 'data_Supplier'
    }}
]).pretty()