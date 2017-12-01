import * as React from 'react';
import PropTypes from 'prop-types';
import shortid from 'shortid';

export default class References extends React.Component {
    static propTypes = {
        references: PropTypes.array.isRequired,
        cleanLink: PropTypes.func.isRequired
    }

    render() {
        let { references, cleanLink } = this.props;
        
        const formatReference = ref => {
            return {
                "link": cleanLink(ref.link),
                "bias": (ref.hasOwnProperty('bias') && ref.bias !== null) 
                    ? ref.bias.biasType
                    : null
            };
        };

        return (
            references
                .map(ref => formatReference(ref))
                .map(ref => (
                    <tr key={shortid.generate()} style={{width: "100%"}}>
                        <td>
                            <a href={ref.link} target="_blank" > {cleanLink(ref.link)} </a>
                        </td>
                        <td>
                            {ref.bias}
                        </td>
                    </tr>
                )   
            )        
        );
    }
}