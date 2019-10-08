db.Person.aggregate([
    {"$match": {
        $expr: {
            $in: ['$age', [26, 27, 28, 29]]
        }
    }}
]).pretty()