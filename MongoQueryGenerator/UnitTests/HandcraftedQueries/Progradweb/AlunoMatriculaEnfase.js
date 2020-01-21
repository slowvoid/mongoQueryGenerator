db.Aluno.aggregate([
    {"$lookup": {
        from: 'Matriculados',
        let: {
            alunoId: '$_id'
        },
        as: 'data_Matriculado',
        pipeline: [
            {$match: {
                $expr: {
                    $eq: ['$cod_aluno', '$$alunoId']
                }
            }},
            {$lookup: {
                from: 'Matricula',
                foreignField: '_id',
                localField: 'cod_matricula',
                as: 'data_Matricula'
            }},
            {$unwind: '$data_Matricula'},
            {
                $addFields: {
                Matriculado_matriculadoId: '$_id',
                Matricula_codmatr_matr: '$data_Matricula._id',
                Matricula_anoini_matr: '$data_Matricula.anoini_matr',
                Matricula_semiini_matr: '$data_Matricula.semiini_matr'
            }},
            {$lookup: {
                from: 'Enfase',
                foreignField: '_id',
                localField: 'cod_enfase',
                as: 'data_Enfase'
            }},
            {$unwind: '$data_Enfase'},
            {$addFields: {
                Enfase_codenf_enf: '$data_Enfase._id',
                Enfase_nomeenf_enf: '$data_Enfase.nomeenf_enf',
                Enfase_siglaenf_enf: '$data_Enfase.siglaenf_enf'
            }},
            {$project: {
                data_Matricula: false,
                data_Enfase: false,
                cod_aluno: false,
                cod_matricula: false,
                cod_enfase: false
            }}
        ]
    }}
]).pretty()