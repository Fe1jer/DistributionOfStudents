import * as yup from 'yup';

export const DistributionValidationSchema = yup.object({
    distributedPlans: yup.array().of(
        yup.object().shape({
            planId: yup.number().min(0).required(),
            passingScore: yup.number().min(0).required(),
            planCount: yup.number().min(0).required(),
            distributedStudents: yup.array().of(
                yup.object().shape({
                    studentId: yup.number().min(0).required(),
                    isDistributed: yup.boolean().required(),
                }),
            ).test('values-test', 'Веберить верное количество абитуриентов', (value, validationContext) => {
                const {
                    createError,
                    parent: {
                        planCount,
                    },
                } = validationContext;
                if (planCount < value.length
                    && value.filter(i => i.isDistributed).length != planCount) {
                    return createError({ message: 'Выберите ' + planCount + ' абитуриентов' });
                }
                return true;
            }),
        })
    )
});