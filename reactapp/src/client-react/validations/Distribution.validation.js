import * as yup from 'yup';

export const DistributionValidationSchema = yup.object({
    distributedPlans: yup.array().of(
        yup.object().shape({
            id: yup.string().uuid(),
            passingScore: yup.number().min(0).required(),
            count: yup.number().min(0).required(),
            distributedStudents: yup.array().of(
                yup.object().shape({
                    id: yup.string().uuid(),
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