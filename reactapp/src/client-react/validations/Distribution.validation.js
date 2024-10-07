import * as yup from 'yup';

export const DistributionValidationSchema = yup.object({
    distributedPlans: yup.array().of(
        yup.object().shape({
            id: yup.string().matches(/^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$/i),
            passingScore: yup.number().min(0).required(),
            count: yup.number().min(0).required(),
            distributedStudents: yup.array().of(
                yup.object().shape({
                    id: yup.string().matches(/^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$/i),
                    isDistributed: yup.boolean().required(),
                }),
            ).test('values-test', 'Веберить верное количество абитуриентов', (value, validationContext) => {
                const {
                    createError,
                    parent: {
                        count,
                    },
                } = validationContext;
                if (count < value.length
                    && value.filter(i => i.isDistributed).length !== count) {
                    return createError({ message: 'Выберите ' + count + ' абитуриентов' });
                }
                return true;
            }),
        })
    )
});